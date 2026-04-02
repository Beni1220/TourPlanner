import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TourService, Tour } from './services/tour.services';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div style="padding: 20px;">
      <h1>Tour Planner</h1>
      
      <!-- Fehler anzeigen -->
      <div *ngIf="error()">
        <p style="color: red;">{{ error() }}</p>
      </div>

      <!-- Liste anzeigen -->
      <div>
        <h2>Touren</h2>
        <ul>
          <li *ngFor="let tour of tours()">
            <strong>{{ tour.name }}</strong> - {{ tour.category }} 
            ({{ tour.distanceKm }}km, {{ tour.durationMinutes }}min)
          </li>
        </ul>
      </div>

      <!-- Neue Tour erstellen -->
      <div style="margin-top: 30px; border: 1px solid #ccc; padding: 10px;">
        <h3>Neue Tour hinzufügen</h3>
        <input placeholder="Name" [(ngModel)]="newTour.name">
        <select [(ngModel)]="newTour.category">
          <option>Bike</option>
          <option>Hike</option>
          <option>Running</option>
          <option>Vacation</option>
        </select>
        <input type="number" placeholder="Distance (km)" [(ngModel)]="newTour.distanceKm">
        <input type="number" placeholder="Duration (min)" [(ngModel)]="newTour.durationMinutes">
        <button (click)="addTour()" style="padding: 10px 20px;">Tour hinzufügen</button>
      </div>
    </div>
  `,
  styleUrl: './app.css'
})
export class App {
  tours = signal<Tour[]>([]);
  error = signal<string>('');
  newTour: Tour = {
    name: '',
    category: 'Bike',
    distanceKm: 0,
    durationMinutes: 0
  };  // ← date raus!

  constructor(private tourService: TourService) {
    this.loadTours();
  }

  loadTours(): void {
    this.tourService.getTours().subscribe({
      next: (tours) => this.tours.set(tours),
      error: (err) => this.error.set('Fehler beim Laden der Touren: ' + err.message)
    });
  }

  addTour(): void {
    if (!this.newTour.name) {
      this.error.set('Bitte Name eingeben!');
      return;
    }

    this.tourService.createTour(this.newTour).subscribe({
      next: (tour) => {
        this.tours.update(t => [...t, tour]);
        this.newTour = {
          name: '',
          category: 'Bike',
          distanceKm: 0,
          durationMinutes: 0
        };  // ← date raus!
        this.error.set('');
      },
      error: (err) => this.error.set('Fehler beim Erstellen: ' + err.message)
    });
  }
}