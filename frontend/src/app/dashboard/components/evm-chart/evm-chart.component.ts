import { Component, input, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseChartDirective } from 'ng2-charts';
import { ChartType, ChartOptions, ChartData } from 'chart.js';
import type { Activity } from '../../../core/models';

@Component({
  selector: 'app-evm-chart',
  standalone: true,
  imports: [CommonModule, BaseChartDirective],
  templateUrl: './evm-chart.component.html',
  styles: [`
    .chart-container {
      @apply bg-white shadow-sm border border-slate-100 rounded-lg p-6;
    }
  `]
})
export class EvmChartComponent implements OnChanges {
  activities = input.required<Activity[]>();

  chartType: ChartType = 'bar';
  chartData: ChartData<'bar'> = {
    labels: [],
    datasets: []
  };
  chartOptions: ChartOptions = {
    responsive: true,
    plugins: {
      legend: {
        position: 'top',
      },
      title: {
        display: true,
        text: 'PV vs EV vs AC por actividad'
      }
    },
    scales: {
      y: {
        beginAtZero: true
      }
    }
  };

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['activities']) {
      this.updateChartData();
    }
  }

  private updateChartData(): void {
    const activities = this.activities();
    
    this.chartData = {
      labels: activities.map(a => a.name),
      datasets: [
        {
          label: 'PV (Planned Value)',
          data: activities.map(a => a.evmIndicators.pv || 0),
          backgroundColor: '#3B82F6',
          borderColor: '#3B82F6',
          borderWidth: 1
        },
        {
          label: 'EV (Earned Value)',
          data: activities.map(a => a.evmIndicators.ev || 0),
          backgroundColor: '#10B981',
          borderColor: '#10B981',
          borderWidth: 1
        },
        {
          label: 'AC (Actual Cost)',
          data: activities.map(a => a.evmIndicators.ac || 0),
          backgroundColor: '#F59E0B',
          borderColor: '#F59E0B',
          borderWidth: 1
        }
      ]
    };
  }
}
