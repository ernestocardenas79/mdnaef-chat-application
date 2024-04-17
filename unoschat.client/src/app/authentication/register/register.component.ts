import { Component } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { AuthenticationService } from '../../core/services/authentication.service';

export interface IUser{
  user: string;
  password: string;
  confirm: string;
}

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {

  salida={};
  constructor(private fb: FormBuilder, private authenticationService:AuthenticationService) {}

  registerForm = this.fb.group({
    user: this.fb.control<string>('', Validators.required),
    password: this.fb.control<string>('', Validators.required),
    confirm: this.fb.control<string>('', Validators.required),
  })

  register(){
    let {value, status} = this.registerForm;
    this.salida= {...value, status};
    this.authenticationService.registerUser((value as IUser));
  }

  get invalidForm(){
    return this.registerForm.status ==='INVALID'?true:false;
  }
}
