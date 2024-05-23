import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthenticationService } from '../services/authentication.service';
import { Router } from '@angular/router';
import { ILogin } from '../interfaces/login.Interface';
import { IUser } from '../interfaces';
import { mergeMap, tap } from 'rxjs/operators';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  loginFailed = false;
  constructor(
    private fb: FormBuilder,
    public auth: AuthenticationService,
    private router: Router
  ) {}

  loginForm = this.fb.group({
    userName: this.fb.control<string>('', Validators.required),
    password: this.fb.control<string>('', Validators.required),
  });

  login() {
    let { value } = this.loginForm;
    this.auth
      .loginUser(value as ILogin)
      .pipe(
        tap((res) => localStorage.setItem('token', res.token)),
        mergeMap(({ email }) => this.auth.userInformation(email))
      )
      .subscribe({
        next: (res) => {
          this.loginFailed = false;
          this.auth.setAuthChange({ ...res });
          this.router.navigate(['/']);
        },
        error: () => {
          this.loginFailed = true;
        },
      });
  }

  get invalidForm() {
    return this.loginForm.status === 'INVALID' ? true : false;
  }

  get IsAuthenticated() {
    return;
  }
}
