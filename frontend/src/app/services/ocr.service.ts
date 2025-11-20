import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface ReceiptItem {
  name: string;
  price: number;
}

export interface ReceiptData {
  merchantName: string;
  totalAmount: number;
  transactionDate: string;
  suggestedCategoryName?: string;
  items?: ReceiptItem[];
  rawText?: string;
}

@Injectable({
  providedIn: 'root'
})
export class OcrService {
  private apiUrl = `${environment.apiUrl}/receipts`;

  constructor(private http: HttpClient) { }

  scanReceipt(file: File): Observable<ReceiptData> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<ReceiptData>(`${this.apiUrl}/scan`, formData);
  }
}
