import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { ExpensesComponent } from './pages/expenses/expenses.component';
import { ScanReceiptComponent } from './pages/scan-receipt/scan-receipt.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
    { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    {
        path: 'dashboard',
        component: DashboardComponent,
        canActivate: [authGuard]
    },
    {
        path: 'expenses',
        component: ExpensesComponent,
        canActivate: [authGuard]
    },
    {
        path: 'scan-receipt',
        component: ScanReceiptComponent,
        canActivate: [authGuard]
    }
];
