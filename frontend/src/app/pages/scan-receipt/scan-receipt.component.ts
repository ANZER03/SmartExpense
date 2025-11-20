import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { OcrService, ReceiptData } from '../../services/ocr.service';
import { ExpenseService, Expense } from '../../services/expense.service';
import { ExpenseFormComponent } from '../../components/expense-form/expense-form.component';

@Component({
  selector: 'app-scan-receipt',
  standalone: true,
  imports: [CommonModule, ExpenseFormComponent],
  templateUrl: './scan-receipt.component.html',
  styleUrl: './scan-receipt.component.scss'
})
export class ScanReceiptComponent {
  selectedFile: File | null = null;
  previewUrl: string | null = null;
  scanning: boolean = false;
  error: string = '';

  scannedData: ReceiptData | null = null;
  expenseFromReceipt: Partial<Expense> | null = null;

  constructor(
    private ocrService: OcrService,
    private expenseService: ExpenseService,
    private router: Router
  ) { }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;

      // Create preview
      const reader = new FileReader();
      reader.onload = (e) => this.previewUrl = e.target?.result as string;
      reader.readAsDataURL(file);

      this.scannedData = null;
      this.expenseFromReceipt = null;
      this.error = '';
    }
  }

  scanReceipt() {
    if (!this.selectedFile) return;

    this.scanning = true;
    this.error = '';

    this.ocrService.scanReceipt(this.selectedFile).subscribe({
      next: (data) => {
        this.scannedData = data;
        this.scanning = false;
        this.prepareExpenseForm(data);
      },
      error: (err) => {
        this.error = 'Failed to scan receipt. Please try again.';
        this.scanning = false;
        console.error(err);
      }
    });
  }

  prepareExpenseForm(data: ReceiptData) {
    // Map receipt data to expense format
    this.expenseFromReceipt = {
      description: data.merchantName || 'Unknown Merchant',
      amount: data.totalAmount || 0,
      date: data.transactionDate ? new Date(data.transactionDate).toISOString().substring(0, 10) : new Date().toISOString().substring(0, 10),
      categoryId: 0 // User needs to select category
    };
  }

  onSaveExpense(expenseData: Partial<Expense>) {
    this.expenseService.createExpense(expenseData).subscribe({
      next: () => {
        this.router.navigate(['/expenses']);
      },
      error: (err) => {
        this.error = 'Failed to create expense';
      }
    });
  }

  onCancel() {
    this.scannedData = null;
    this.expenseFromReceipt = null;
  }
}
