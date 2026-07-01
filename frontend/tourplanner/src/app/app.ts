import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TourService, Tour } from './services/tour.services';
import { MapComponent } from './components/map/map';
import { TourForm } from './components/tour-form/tour-form';
import { TourLogs } from './components/tour-logs/tour-logs';
import { User } from './components/user/user';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule, MapComponent, TourForm, TourLogs, User],
  templateUrl: './app.html',
  styleUrl: './app.css'
})

export class App {
  constructor(public auth: AuthService) {}
}
