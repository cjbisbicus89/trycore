import { Component, input, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseChartDirective } from 'ng2-charts';
import { ChartType, ChartOptions, ChartData } from 'chart.js';
import type { Activity } from '../../../core/models';

@Component({
  selector: 'app-evm-chart',
  standalone: true,
  imports: [CommonModule, BaseChartDirective],
  templateUrl: './evm-chart.component.html',
  styles: []
})
export class EvmChartComponent {
  activities = input.required<Activity[]>();

  chartType: ChartType = 'bar';
  chartData: ChartData<'bar'> = {
    labels: [],
    datasets: []
  };
  chartOptions: ChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        position: 'top',
        labels: {
          font: { family: 'Inter', size: 12 },
          color: '#1A2B4A'
        }
      },
      title: {
        display: true,
        text: 'PV vs EV vs AC por actividad',
        font: { family: 'Inter', size: 14, weight: 600 },
        color: '#1A2B4A'
      }
    },
    scales: {
      y: {
        beginAtZero: true,
        ticks: { font: { family: 'Inter', size: 11 }, color: '#64748B' },
        grid: { color: '#F1F5F9' }
      },
      x: {
        ticks: { font: { family: 'Inter', size: 11 }, color: '#64748B' },
        grid: { display: false }
      }
    }
  };

  constructor() {
    effect(() => {
      const activities = this.activities();
      this.chartData = {
        labels: activities.map(a => a.name),
        datasets: [
          {
            label: 'PV — Planned Value',
            data: activities.map(a => a.plannedValue),
            backgroundColor: '#1E3A6E',
            borderColor: '#1A2B4A',
            borderWidth: 1
          },
          {
            label: 'EV — Earned Value',
            data: activities.map(a => a.earnedValue),
            backgroundColor: '#FF6B2B',
            borderColor: '#CC5522',
            borderWidth: 1
          },
          {
            label: 'AC — Actual Cost',
            data: activities.map(a => a.actualCost),
            backgroundColor: '#94A3B8',
            borderColor: '#64748B',
            borderWidth: 1
          }
        ]
      };
    });
  }
}
