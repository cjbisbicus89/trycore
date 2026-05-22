import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatDecimal',
  standalone: true
})
export class FormatDecimalPipe implements PipeTransform {
  transform(value: number | null | undefined, decimals: number = 2): string {
    if (value === null || value === undefined) {
      return '—';
    }
    return value.toFixed(decimals);
  }
}
