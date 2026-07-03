import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AuthService {
  showLogin = signal(false);
  username: string = "";

  open()  { this.showLogin.set(true);  }
  close() { this.showLogin.set(false); }

  isLoggedIn = signal(localStorage.getItem('token') !== null);

  userLoggedIn() {
    this.isLoggedIn.set(true);
  }
}