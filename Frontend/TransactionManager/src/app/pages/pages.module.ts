import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PagesRoutingModule } from './pages-routing.module';
import { LoginComponent } from './login/login.component';
import { CallbackComponent } from './login/OAuth/OAuth_Callback/callback.component';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { MatCardModule } from '@angular/material/card'
import { MatFormFieldModule } from '@angular/material/form-field';
import { DashboardComponent } from './main/dashboard/dashboard.component';
import { ReportComponent } from './main/report/report.component';
import { TransactionComponent } from './main/transaction/transaction.component';
import { ImgurComponent } from './login/OAuth/imgur.component';
const componentList = [
  LoginComponent,
  CallbackComponent,
  ImgurComponent,
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
