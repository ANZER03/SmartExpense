import { Component, Input, OnChanges, SimpleChanges, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseChartDirective, provideCharts, withDefaultRegisterables } from 'ng2-charts';
import { ChartConfiguration, ChartData, ChartEvent, ChartType } from 'chart.js';

@Component({
  selector: 'app-expense-by-category-chart',
  standalone: true,
  imports: [CommonModule, BaseChartDirective],
  providers: [provideCharts(withDefaultRegisterables())],
  templateUrl: './expense-by-category-chart.component.html',
  styleUrl: './expense-by-category-chart.component.scss'
})
export class ExpenseByCategoryChartComponent implements OnChanges {
  @Input() data: { categoryName: string; amount: number; color: string }[] = [];

  @ViewChild(BaseChartDirective) chart: BaseChartDirective | undefined;

  public doughnutChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        display: true,
        position: 'right',
        labels: {
          usePointStyle: true,
          padding: 20,
          font: {
            family: "'Inter', sans-serif",
            size: 12
          }
        }
      },
      tooltip: {
        callbacks: {
          label: (context) => {
            const label = context.label || '';
            const value = context.raw || 0;
            const total = context.chart.data.datasets[0].data.reduce((a: any, b: any) => a + b, 0) as number;
            const percentage = Math.round(((value as number) / total) * 100);
            return ` ${label}: $${value} (${percentage}%)`;
          }
        }
      }
    }
  };

  public doughnutChartData: ChartData<'doughnut'> = {
    labels: [],
    datasets: [
      {
        data: [],
        backgroundColor: [],
        hoverOffset: 4,
        borderWidth: 0
      }
    ]
  };

  public doughnutChartType: ChartType = 'doughnut';

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['data'] && this.data) {
      this.updateChartData();
    }
  }

  private updateChartData(): void {
    this.doughnutChartData = {
      labels: this.data.map(d => d.categoryName),
      datasets: [
        {
          data: this.data.map(d => d.amount),
          backgroundColor: this.data.map(d => d.color),
          hoverOffset: 4,
          borderWidth: 0
        }
      ]
    };

    this.chart?.update();
  }
}
