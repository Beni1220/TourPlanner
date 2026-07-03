import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ITourLogs {
  id?: number;
  tourId: number;
  date: Date;
  comment: string;
  difficulty: number; // 0=Easy, 1=Medium, 2=Hard
  rating: number;
  totalDistance: number;
  totalTime: number;
}

@Injectable({
  providedIn: 'root'
})
export class TourLogsService {

  
  private apiUrl = '/api/tourlogs';

  constructor(private http: HttpClient) { }

  getToursLogs(): Observable<ITourLogs[]> {
    const token = localStorage.getItem('token'); // Retrieve the token from local storage
    return this.http.get<ITourLogs[]>(`${this.apiUrl}/token`, { headers: { Authorization: `Bearer ${token}` } });
  }
  /*
  getTourLogs(): Observable<ITourLogs[]> {
    return this.http.get<ITourLogs[]>(this.apiUrl);
  }
  */

  getTourNameByTourId(tourId: number): Observable<string> {
    return this.http.get(`${this.apiUrl}/tourName/${tourId}`, { responseType: 'text' });
  }

  createTourLog(tourLog: ITourLogs): Observable<ITourLogs> {
    return this.http.post<ITourLogs>(this.apiUrl, tourLog);
  }

  updateTourLog(tourLog: ITourLogs): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${tourLog.id}`, tourLog);
  }

  deleteTourLog(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
} 