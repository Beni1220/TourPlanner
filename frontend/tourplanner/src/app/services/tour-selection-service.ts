import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TourSelectionService {
  selectedTourId = signal<number>(0); // Signal to hold the selected tour ID
  selectedTourName = signal<string>(''); // Signal to hold the selected tour name
}
