import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExpenseService, DashboardData } from '../../services/expense.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
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
