import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TourService, Tour } from './services/tour.services';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <main class="app-shell">
      <section class="hero-panel">
        <div>
          <span class="eyebrow">Tour Planner</span>
          <h1>Deine Touren verwalten</h1>
          <p>Min = Minuten, km = Kilometer. Erstelle, bearbeite oder lösche deine Touren direkt hier.</p>
        </div>
      </section>

      <div *ngIf="error()" class="toast error-toast">
        {{ error() }}
      </div>

      <section class="create-card">
        <div class="card-header">
          <div>
            <p class="small-label">Neue Tour</p>
            <h2>Tour hinzufügen</h2>
          </div>
        </div>

        <div class="form-row">
          <label>Name</label>
          <input type="text" placeholder="Tourname" [(ngModel)]="newTour.name" />
        </div>

        <div class="form-row">
          <label>Kategorie</label>
          <select [(ngModel)]="newTour.category">
            <option>Bike</option>
            <option>Hike</option>
            <option>Running</option>
            <option>Vacation</option>
          </select>
        </div>

        <div class="form-grid">
          <div class="form-row">
            <label>Distance (km)</label>
            <input type="number" min="0" [(ngModel)]="newTour.distanceKm" />
          </div>
          <div class="form-row">
            <label>Duration (min)</label>
            <input type="number" min="0" [(ngModel)]="newTour.durationMinutes" />
          </div>
        </div>

        <p class="hint">Gib die Streckenlänge in km und die Dauer in Minuten ein.</p>
        <button class="primary-button" (click)="addTour()">Tour hinzufügen</button>
      </section>

      <section class="list-section">
        <div class="section-header">
          <div>
            <h2>Touren</h2>
            <p class="section-hint">Bearbeite Einträge direkt oder lösche veraltete Touren.</p>
          </div>
        </div>

        <div class="tour-grid">
          <article *ngFor="let tour of tours()" class="tour-card">
            <ng-container *ngIf="editingTour?.id === tour.id; then editMode else viewMode"></ng-container>

            <ng-template #editMode>
              <div class="card-row" *ngIf="editingTour as currentEdit">
                <input type="text" [(ngModel)]="currentEdit.name" />
                <select [(ngModel)]="currentEdit.category">
                  <option>Bike</option>
                  <option>Hike</option>
                  <option>Running</option>
                  <option>Vacation</option>
                </select>
              </div>
              <div class="card-row" *ngIf="editingTour as currentEdit">
                <input type="number" min="0" [(ngModel)]="currentEdit.distanceKm" />
                <input type="number" min="0" [(ngModel)]="currentEdit.durationMinutes" />
              </div>
              <div class="card-actions">
                <button class="secondary-button" (click)="saveEdit()">Speichern</button>
                <button class="danger-button" (click)="cancelEdit()">Abbrechen</button>
              </div>
            </ng-template>

            <ng-template #viewMode>
              <div class="tour-title">{{ tour.name }}</div>
              <div class="tour-meta">{{ tour.category }} · {{ tour.distanceKm }} km · {{ tour.durationMinutes }} min</div>
              <div class="card-actions">
                <button class="secondary-button" (click)="startEdit(tour)">Bearbeiten</button>
                <button class="danger-button" (click)="deleteTour(tour.id!)">Löschen</button>
              </div>
            </ng-template>
          </article>
        </div>
      </section>
    </main>
  `,
  styleUrl: './app.css'
})
export class App {
  tours = signal<Tour[]>([]);
  error = signal<string>('');
  editingTour: Tour | null = null;

  newTour: Tour = {
    name: '',
    category: 'Bike',
    distanceKm: 0,
    durationMinutes: 0
  };

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
    if (!this.newTour.name.trim()) {
      this.error.set('Bitte Name eingeben!');
      return;
    }

    this.tourService.createTour(this.newTour).subscribe({
      next: (tour) => {
        this.tours.update(t => [...t, tour]);
        this.resetNewTour();
        this.error.set('');
      },
      error: (err) => this.error.set('Fehler beim Erstellen: ' + err.message)
    });
  }

  startEdit(tour: Tour): void {
    this.editingTour = { ...tour };
    this.error.set('');
  }

  cancelEdit(): void {
    this.editingTour = null;
  }

  saveEdit(): void {
    if (!this.editingTour || this.editingTour.id == null) {
      return;
    }

    if (!this.editingTour.name.trim()) {
      this.error.set('Name darf nicht leer sein.');
      return;
    }

    this.tourService.updateTour(this.editingTour).subscribe({
      next: () => {
        this.tours.update(list => list.map(t => t.id === this.editingTour?.id ? { ...this.editingTour! } : t));
        this.editingTour = null;
        this.error.set('');
      },
      error: (err) => this.error.set('Fehler beim Aktualisieren: ' + err.message)
    });
  }

  deleteTour(id: number): void {
    this.tourService.deleteTour(id).subscribe({
      next: () => {
        this.tours.update(list => list.filter(t => t.id !== id));
        if (this.editingTour?.id === id) {
          this.editingTour = null;
        }
        this.error.set('');
      },
      error: (err) => this.error.set('Fehler beim L�schen: ' + err.message)
    });
  }

  private resetNewTour(): void {
    this.newTour = {
      name: '',
      category: 'Bike',
      distanceKm: 0,
      durationMinutes: 0
    };
  }
}
