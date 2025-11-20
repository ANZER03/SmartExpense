import { Component, Input, OnChanges, SimpleChanges, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseChartDirective, provideCharts, withDefaultRegisterables } from 'ng2-charts';
import { ChartConfiguration, ChartData, ChartEvent, ChartType } from 'chart.js';

@Component({
  selector: 'app-expense-by-month-chart',
  standalone: true,
  imports: [CommonModule, BaseChartDirective],
  providers: [provideCharts(withDefaultRegisterables())],
  templateUrl: './expense-by-month-chart.component.html',
  styleUrl: './expense-by-month-chart.component.scss'
})
export class ExpenseByMonthChartComponent implements OnChanges {
  @Input() data: any[] = [];

  @ViewChild(BaseChartDirective) chart: BaseChartDirective | undefined;

  public barChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    scales: {
      x: {
        grid: {
          display: false
        },
        ticks: {
          font: {
            family: "'Inter', sans-serif"
          }
        }
      },
      y: {
        beginAtZero: true,
        grid: {
          color: 'rgba(0, 0, 0, 0.05)'
        },
        ticks: {
          font: {
            family: "'Inter', sans-serif"
          },
          callback: function (value) {
            return '$' + value;
          }
        }
      }
    },
    plugins: {
      legend: {
        display: false
      },
      tooltip: {
        callbacks: {
          label: (context) => {
            return ` Expenses: $${context.raw}`;
          }
        }
      }
    }
  };

  public barChartData: ChartData<'bar'> = {
    labels: [],
    datasets: [
      {
        data: [],
        label: 'Expenses',
        backgroundColor: 'rgba(0, 0, 0, 0.8)',
        hoverBackgroundColor: 'rgba(0, 0, 0, 1)',
        borderRadius: 4,
        barThickness: 20
      }
    ]
  };

  public barChartType: ChartType = 'bar';

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['data'] && this.data) {
      this.updateChartData();
    }
  }

  private updateChartData(): void {
    const labels = this.data.map(d => d.month || d.date);
    const amounts = this.data.map(d => d.amount);

    this.barChartData = {
      labels: labels,
      datasets: [
        {
          data: amounts,
          label: 'Expenses',
          backgroundColor: 'rgba(0, 0, 0, 0.8)',
          hoverBackgroundColor: 'rgba(0, 0, 0, 1)',
          borderRadius: 4,
          barThickness: 20
        }
      ]
    };

    this.chart?.update();
  }
}
