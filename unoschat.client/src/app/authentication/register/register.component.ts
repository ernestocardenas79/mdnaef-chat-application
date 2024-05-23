import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthenticationService } from '../../core/services/authentication.service';
import { Router } from '@angular/router';
import { IAuthUser, IUser } from '../../core/interfaces';
import { mergeMap, tap } from 'rxjs';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  registartionFailed = false;
  salida = {};
  constructor(
    private fb: FormBuilder,
    public auth: AuthenticationService,
    private router: Router
  ) {}

  registerForm = this.fb.group({
    name: this.fb.control<string>('', Validators.required),
    email: this.fb.control<string>('', Validators.required),
    password: this.fb.control<string>('', Validators.required),
    confirmPassword: this.fb.control<string>('', Validators.required),
  });

  register() {
    let { value, status } = this.registerForm;
    this.salida = { ...value, status };
    this.auth
      .registerUser(value as IAuthUser)
      .pipe(
        tap((res) => {
          localStorage.setItem('token', res.token);
          console.log('res', res);
        }),
        mergeMap(({ email }) => this.auth.userInformation(email))
      )
      .subscribe({
        next: (res) => {
          console.log('Nregister', res);
          this.registartionFailed = false;
          this.auth.setAuthChange(res);
          this.router.navigate(['/']);
        },
        error: () => {
          this.registartionFailed = true;
        },
      });
  }

  get invalidForm() {
    return this.registerForm.status === 'INVALID' ? true : false;
  }
}
