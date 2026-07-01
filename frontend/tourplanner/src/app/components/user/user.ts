import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './user.html',
  styleUrl: './user.css'
})
export class User {
  activeTab: 'login' | 'register' = 'login';
  showPassword = false;

  loginForm = { username: '', password: '' };
  registerForm = { username: '', password: '' };

  constructor(private http: HttpClient, public auth: AuthService) {}

  login() {
    this.http.post('/api/users/login', this.loginForm).subscribe({
      next: (res: any) => {
        localStorage.setItem('token', res.token);
        this.auth.close(); 
      },
      error: (err) => console.error(err)
    });
  }

  register() {
    this.http.post('/api/users/register', this.registerForm).subscribe({
      next: (res: any) => {
        localStorage.setItem('token', res.token);
        this.auth.close();
      },
      error: (err) => console.error(err)
    });
  }

  get passwordStrength(): number {
    const p = this.registerForm.password;
    let s = 0;
    if (p.length >= 6) s++;
    if (p.length >= 10) s++;
    if (/[A-Z]/.test(p)) s++;
    if (/[0-9]/.test(p)) s++;
    return s;
  }
}

