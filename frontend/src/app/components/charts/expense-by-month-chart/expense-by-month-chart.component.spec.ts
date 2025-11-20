import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpenseByMonthChartComponent } from './expense-by-month-chart.component';

describe('ExpenseByMonthChartComponent', () => {
  let component: ExpenseByMonthChartComponent;
  let fixture: ComponentFixture<ExpenseByMonthChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ExpenseByMonthChartComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ExpenseByMonthChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
