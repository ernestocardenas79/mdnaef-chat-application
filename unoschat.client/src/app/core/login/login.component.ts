import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthenticationService } from '../services/authentication.service';

export interface ILogin {
  user: string;
  password: string;
}

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {  
  constructor(private fb: FormBuilder, private authenticationService:AuthenticationService) {}

  loginForm = this.fb.group({
    user: this.fb.control<string>('', Validators.required),
    password: this.fb.control<string>('', Validators.required),
  })

  login(){
    let {value} = this.loginForm;
    this.authenticationService.loginUser((value as ILogin));
  }

  get invalidForm(){
    return this.loginForm.status ==='INVALID'?true:false;
  }
}
