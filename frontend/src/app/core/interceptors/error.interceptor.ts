import {
  HttpRequest,
  HttpHandlerFn,
  HttpEvent,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { inject } from '@angular/core';
import { ErrorService } from '../services/error.service';

export const errorInterceptor = (request: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> => {
  const errorService = inject(ErrorService);

  return next(request).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMessage = 'An error occurred';

      if (error.error instanceof ErrorEvent) {
        // Client-side error
        errorMessage = `Error: ${error.error.message}`;
      } else {
        // Server-side error
        switch (error.status) {
          case 400: {
            const errorBody = error.error as { errors?: Record<string, string[]>; message?: string };
            if (errorBody?.errors) {
              const errorMessages = Object.values(errorBody.errors).flat();
              errorMessage = errorMessages.join(', ');
            } else if (errorBody?.message) {
              errorMessage = errorBody.message;
            } else {
              errorMessage = 'Solicitud inválida';
            }
            break;
          }
          case 404:
            errorMessage = 'Recurso no encontrado';
            break;
          case 500:
            errorMessage = 'Error interno del servidor';
            break;
          default:
            errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
        }
      }

      // Usar signal global para mostrar el error
      errorService.setError(errorMessage);

      return throwError(() => errorMessage);
    })
  );
};
