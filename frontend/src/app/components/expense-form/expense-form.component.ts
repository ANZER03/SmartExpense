import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ExpenseService, Category, Expense } from '../../services/expense.service';

@Component({
  selector: 'app-expense-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './expense-form.component.html',
  styleUrl: './expense-form.component.scss'
})
export class ExpenseFormComponent implements OnInit {
  @Input() expense: Partial<Expense> | null = null;
  @Output() save = new EventEmitter<Partial<Expense>>();
  @Output() cancel = new EventEmitter<void>();

  expenseForm: FormGroup;
  categories: Category[] = [];
  loading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private expenseService: ExpenseService
  ) {
    this.expenseForm = this.fb.group({
      description: ['', [Validators.required]],
      amount: [null, [Validators.required, Validators.min(0.01)]],
      date: [new Date().toISOString().substring(0, 10), [Validators.required]],
      categoryId: [null, [Validators.required]]
    });
  }

  ngOnInit() {
    this.loadCategories();
    if (this.expense) {
      const dateValue = this.expense.date
        ? new Date(this.expense.date).toISOString().substring(0, 10)
        : new Date().toISOString().substring(0, 10);

      this.expenseForm.patchValue({
        description: this.expense.description,
        amount: this.expense.amount,
        date: dateValue,
        categoryId: this.expense.categoryId
      });
    }
  }

  loadCategories() {
    this.expenseService.getCategories().subscribe(categories => {
      this.categories = categories;
    });
  }

  onSubmit() {
    if (this.expenseForm.invalid) return;
    this.save.emit(this.expenseForm.value);
  }

  onCancel() {
    this.cancel.emit();
  }
}
