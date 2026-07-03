import { Component, effect, signal } from '@angular/core';
import { TourService, Tour } from '../../services/tour.services';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { OpenrouteService } from '../../services/openroute.service';
import {AuthService} from "../../services/auth.service";
import { ErrorHandlingService } from '../../services/ErrorHandlingService';


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
  selectedTourId: number | null = null;

  newTour: Tour = {
    id: undefined,
    name: '',
    description: '',
    from: '',
    to: '',
    transportType: 'Bike',
    tourDistance: 0,
    estimatedTime: 0,
  };

  fromValid = signal<boolean | null>(null);
  toValid = signal<boolean | null>(null);
  
  /*
  ngOnInit() {
    this.loadTours();
  } 
  */

  constructor(private tourService: TourService, private openRouteService: OpenrouteService, public authService: AuthService, private errorHandlingService: ErrorHandlingService) {
    effect(() => {
      console.log('AuthService isLoggedIn changed:', this.authService.isLoggedIn());
      // läuft automatisch jedes Mal, wenn sich authService.isLoggedIn() ändert
      if (this.authService.isLoggedIn()) {
        this.loadTours();
      }
    });
  }

  public validatePlace(place: string, type: 'from' | 'to') {
    if (!place.trim()) {
      if (type === 'from') this.fromValid.set(null);
      else this.toValid.set(null);
      return;
    }

    setTimeout(async () => {
      const valid = await this.openRouteService.validatePlace(place);
      if (type === 'from') this.fromValid.set(valid);
      else this.toValid.set(valid);
    }, 500);
  }

  

  async selectTour(tour: Tour) {
    this.selectedTourId = tour.id!;
    this.tourService.selectedTourId.set(tour.id!); // Set the selected tour ID in the service
    this.tourService.selectedTourName.set(tour.name); // Set the selected tour name in the service
    try {

      const route = await this.openRouteService.getRoute(
        tour.from,
        tour.to,
        tour.id!
      );
      
      this.tourService.tourRouteAdded.next(route);
      
    } catch (err) {

      console.error(err);
      this.error.set('Route konnte nicht geladen werden');

    }

  }

  loadTours(): void {
  this.tourService.getTours().subscribe({
    next: (tours) => this.tours.set(tours),
    error: (err) => this.error.set(this.errorHandlingService.getErrorMessage(err))
  });
}

  async addTour(): Promise<void> {

    if (!this.newTour.name.trim()) {
      this.error.set('Bitte Name eingeben!');
      return;
    }

    try {
      const { coordinates, distance } = await this.openRouteService.loadRouteFromORS(
        this.newTour.from,
        this.newTour.to
      );

      this.newTour.tourDistance = distance; // NEU – Distanz vor dem Erstellen setzen

      this.tourService.createTour(this.newTour).subscribe({
        next: async (tour) => {
          try {
            await this.openRouteService.saveRoute(tour.id!, coordinates);
            this.tourService.tourRouteAdded.next(coordinates);
            this.tours.update(list => [...list, tour]);
            this.resetNewTour();
            this.fromValid.set(null);
            this.toValid.set(null);
            this.error.set('');
          } catch (err) {
            console.error(err);
            this.error.set('Route konnte nicht erstellt werden');
          }
        },
        error: err =>
          this.error.set(this.errorHandlingService.getErrorMessage(err))
      });
    } catch (err) {
      console.error(err);
      this.error.set('Route konnte nicht berechnet werden');
    }

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
      error: (err) => this.error.set(this.errorHandlingService.getErrorMessage(err))
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
      error: (err) => this.error.set(this.errorHandlingService.getErrorMessage(err))
    });
  }

  roundKm(meters: number): number {
    return Math.round(meters / 100) / 10;  // Rundet auf eine Nachkommastelle
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
