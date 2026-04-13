import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TourService, Tour } from './services/tour.services';
import { MapComponent } from './components/map/map';
import { TourForm } from './components/tour-form/tour-form';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule, MapComponent, TourForm],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  error = signal<string>('');
}
