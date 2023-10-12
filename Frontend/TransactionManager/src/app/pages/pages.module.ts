import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './auth/login/login.component';
import { ImgurCallbackComponent } from './auth/OAuth/imgur-callback.component';
import { ImgurComponent } from './auth/OAuth/imgur.component';
import { PagesRoutingModule } from './pages-routing.module';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { MatCardModule } from '@angular/material/card'
import { MatFormFieldModule } from '@angular/material/form-field';
import { DashboardComponent } from './main/dashboard/dashboard.component';
import { ReportComponent } from './main/report/report.component';
import { TransactionComponent } from './main/transaction/transaction.component';
import { AuthComponent } from './auth/auth.component';
const componentList = [
  LoginComponent,
  ImgurCallbackComponent,
  ImgurComponent,
  AuthComponent,
  DashboardComponent,
  ReportComponent,
  TransactionComponent
]
const material = [
  MatCardModule,
  FormsModule,
  MatFormFieldModule
]
@NgModule({
  declarations: [
    componentList,    
  ],
  exports: componentList,
  imports: [
    CommonModule,
    PagesRoutingModule,
    FormsModule,
    BrowserModule,
    material
  ],
})
export class PagesModule { }

