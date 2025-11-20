import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExpenseService, DashboardData } from '../../services/expense.service';

import { ExpenseByCategoryChartComponent } from '../../components/charts/expense-by-category-chart/expense-by-category-chart.component';
import { ExpenseByMonthChartComponent } from '../../components/charts/expense-by-month-chart/expense-by-month-chart.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, ExpenseByCategoryChartComponent, ExpenseByMonthChartComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  dashboardData: DashboardData | null = null;
  loading: boolean = true;
  error: string = '';

  constructor(private expenseService: ExpenseService) { }

  ngOnInit() {
    this.loadDashboardData();
  }

  loadDashboardData() {
    this.loading = true;
    this.expenseService.getDashboardData().subscribe({
      next: (data) => {
        this.dashboardData = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load dashboard data';
        this.loading = false;
        console.error(err);
      }
    });
  }
}
