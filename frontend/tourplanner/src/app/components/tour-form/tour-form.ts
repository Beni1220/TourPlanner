import { Component, signal } from '@angular/core';
import { TourService, Tour } from '../../services/tour.services';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { OpenrouteService } from '../../services/openroute.service';


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
  

  constructor(private tourService: TourService, private openRouteService: OpenrouteService) {
    this.loadTours();
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
      error: (err) => this.error.set('Fehler beim Laden der Touren: ' + err.message)
    });
  }


  async addTour(): Promise<void> {

    if (!this.newTour.name.trim()) {
      this.error.set('Bitte Name eingeben!');
      return;
    }

    this.tourService.createTour(this.newTour).subscribe({

      next: async (tour) => {

        try {

          const route = await this.openRouteService.getRoute(
            tour.from,
            tour.to,
            tour.id!
          );

          this.tourService.tourRouteAdded.next(route);

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
        this.error.set('Fehler beim Erstellen: ' + err.message)

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
