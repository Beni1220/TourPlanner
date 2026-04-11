// tour.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Tour {
  id?: number;
  name: string;
  category: 'Bike' | 'Hike' | 'Running' | 'Vacation';
  distanceKm: number;
  durationMinutes: number;
}

@Injectable({
  providedIn: 'root'
})
export class TourService {

  private apiUrl = '/api/tours';

  constructor(private http: HttpClient) { }

  getTours(): Observable<Tour[]> {
    return this.http.get<Tour[]>(this.apiUrl);
  }

  createTour(tour: Tour): Observable<Tour> {
    return this.http.post<Tour>(this.apiUrl, tour);
  }

  updateTour(tour: Tour): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${tour.id}`, tour);
  }

  deleteTour(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

}