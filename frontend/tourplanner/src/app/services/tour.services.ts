import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';

export interface Tour {
  id?: number;
  name: string;
  description?: string;
  from: string;
  to: string;
  transportType: 'Bike' | 'Hike' | 'Running' | 'Vacation';
  estimatedTime: number;
  tourDistance: number;
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

  tourRouteAdded = new Subject<[number, number][]>();

}