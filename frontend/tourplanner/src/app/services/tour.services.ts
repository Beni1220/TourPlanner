import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { signal } from '@angular/core';


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
  private apiUrl = '/api/tours'

  constructor(private http: HttpClient) { }

  getTours(): Observable<Tour[]> {
    const token = localStorage.getItem('token'); // Retrieve the token from local storage
    return this.http.get<Tour[]>(`${this.apiUrl}/token`, { headers: { Authorization: `Bearer ${token}` } });
  }
 

  createTour(tour: Tour): Observable<Tour> {
    const token = localStorage.getItem('token'); // Retrieve the token from local storage
    return this.http.post<Tour>(this.apiUrl, tour, {headers: { Authorization: `Bearer ${token}`}});
  }

  updateTour(tour: Tour): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${tour.id}`, tour);
  }

  deleteTour(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getTourByToken(): Observable<Tour> {
    const token = localStorage.getItem('token');
    return this.http.get<Tour>(`${this.apiUrl}/editableTours`, {headers: { Authorization: `Bearer ${token}`}});
  }

  exportTours(): Observable<any> {
    const token = localStorage.getItem('token');
    return this.http.get(`${this.apiUrl}/export`, {
      headers: { Authorization: `Bearer ${token}` }
    });
  }

  importTours(tours: any[]): Observable<any> {
    const token = localStorage.getItem('token');
    return this.http.post(`${this.apiUrl}/import`, tours, {
      headers: { Authorization: `Bearer ${token}` }
    });
  }

  searchTour(searchTerm: string): Observable<Tour[]> {
  const token = localStorage.getItem('token');
  const params = new HttpParams().set('searchTerm', searchTerm);
  return this.http.get<Tour[]>(`${this.apiUrl}/search`, {
    headers: { Authorization: `Bearer ${token}` },
    params
  });
}

  tourRouteAdded = new Subject<[number, number][]>();

}