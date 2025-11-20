import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ExpenseService, Expense, Category } from '../../services/expense.service';
import { ExpenseFormComponent } from '../../components/expense-form/expense-form.component';

@Component({
  selector: 'app-expenses',
  standalone: true,
  imports: [CommonModule, FormsModule, ExpenseFormComponent],
  templateUrl: './expenses.component.html',
  styleUrl: './expenses.component.scss'
})
export class ExpensesComponent implements OnInit {
  expenses: Expense[] = [];
  categories: Category[] = [];
  loading: boolean = true;
  error: string = '';

  // Filters
  selectedCategoryId: number | null = null;

  // Modal state
  showModal: boolean = false;
  editingExpense: Expense | null = null;

  constructor(private expenseService: ExpenseService) { }

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.loading = true;
    this.expenseService.getCategories().subscribe(cats => this.categories = cats);
    this.loadExpenses();
  }

  loadExpenses() {
    this.loading = true;
    this.expenseService.getExpenses(this.selectedCategoryId || undefined).subscribe({
      next: (data) => {
        this.expenses = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load expenses';
        this.loading = false;
      }
    });
  }

  onFilterChange() {
    this.loadExpenses();
  }

  openAddModal() {
    this.editingExpense = null;
    this.showModal = true;
  }

  openEditModal(expense: Expense) {
    this.editingExpense = expense;
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.editingExpense = null;
  }

  onSaveExpense(expenseData: Partial<Expense>) {
    if (this.editingExpense) {
      this.expenseService.updateExpense(this.editingExpense.id, expenseData).subscribe(() => {
        this.loadExpenses();
        this.closeModal();
      });
    } else {
      this.expenseService.createExpense(expenseData).subscribe(() => {
        this.loadExpenses();
        this.closeModal();
      });
    }
  }

  deleteExpense(id: number) {
    if (confirm('Are you sure you want to delete this expense?')) {
      this.expenseService.deleteExpense(id).subscribe(() => {
        this.loadExpenses();
      });
    }
  }
}
