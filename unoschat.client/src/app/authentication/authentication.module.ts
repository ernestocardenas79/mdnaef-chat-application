import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterComponent } from './register/register.component';
import { AuthenticationRoutingModule } from './authentication-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';



@NgModule({
  declarations: [
    RegisterComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    AuthenticationRoutingModule,
    HttpClientModule
  ]
})
export class AuthenticationModule { }
