import { Component, signal } from '@angular/core';
import { TourService, Tour } from '../../services/tour.services';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { OpenrouteService } from '../../services/openroute.service';
import { MapComponent } from '../map/map';


@Component({
  selector: 'app-tour-form',
  imports: [CommonModule, FormsModule],
  templateUrl: './tour-form.html',
  styleUrl: './tour-form.css',
})
export class TourForm {
  tours = signal<Tour[]>([]);
  error = signal<string>('');
  editingTour: Tour | null = null;

  newTour: Tour = {
    name: '',
    description: '',
    from: '',
    to: '',
    transportType: 'Bike',
    tourDistance: 0,
    estimatedTime: 0,
  };
  route: [[number, number], [number, number]] = [[0, 0], [0, 0]];

  constructor(private tourService: TourService, private openRouteService: OpenrouteService) {
    this.loadTours();
  }

  async createRoutes() {
    try {
      const from = await this.openRouteService.getCoordinates(this.newTour.from);
      const to = await this.openRouteService.getCoordinates(this.newTour.to);

      this.route = [from, to];

    } catch (err) {
      console.error(err);
    }
  }


  loadTours(): void {
    this.tourService.getTours().subscribe({
      next: (tours) => this.tours.set(tours),
      error: (err) => this.error.set('Fehler beim Laden der Touren: ' + err.message)
    });
  }


  async addTour(): Promise<void> {
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
    await this.createRoutes();
    
    // Notify the map component to update the route
    this.tourService.tourAdded.next(this.route);
    //this.mapComponent.getRoute(this.route);
    
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
      error: (err) => this.error.set('Fehler beim Löschen: ' + err.message)
    });
  }

  private resetNewTour(): void {
    this.newTour = {
      name: '',
      description: '',
      from: '',
      to: '',
      transportType: 'Bike',
      estimatedTime: 0,
      tourDistance: 0,
    };
  }
}
