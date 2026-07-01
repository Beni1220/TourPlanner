
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';

export interface TourCoordinate {
  id?: number;
  tourId: number;
  latitude: number;
  longitude: number;
  sequence: number;
}

@Injectable({
  providedIn: 'root'
})
export class OpenrouteService {

  private apiKey = 'eyJvcmciOiI1YjNjZTM1OTc4NTExMTAwMDFjZjYyNDgiLCJpZCI6ImE1MGFlMzlmZDcwYzQ4NDY5MWE3NzgxNDc4OWJhZTQwIiwiaCI6Im11cm11cjY0In0=';
  private apiUrl = '/api/tourcoordinates';

  constructor(private http: HttpClient) {}

  async getRoute(
    from: string,
    to: string,
    tourId: number
  ): Promise<[number, number][]> {

    // zuerst Backend
    const backendRoute = await this.loadRouteFromBackend(tourId);

    if (backendRoute.length > 0) {
      console.log('Route aus Backend geladen');
      return backendRoute;
    }

    console.log('Route von ORS laden');

    const orsRoute = await this.loadRouteFromORS(from, to);

    await this.saveRoute(tourId, orsRoute);

    return orsRoute;
  }

  private async loadRouteFromBackend(
    tourId: number
  ): Promise<[number, number][]> {

    try {

      const coords = await firstValueFrom(
        this.http.get<TourCoordinate[]>(`${this.apiUrl}/${tourId}`)
      );

      return coords.map(c => [c.longitude, c.latitude]);

    } catch {

      return [];

    }
  }

  private async loadRouteFromORS(
    from: string,
    to: string
  ): Promise<[number, number][]> {

    const start = await this.getCoordinates(from);
    const end = await this.getCoordinates(to);

      console.log('Start:', start);
      console.log('End:', end);
    const response = await fetch(
      'https://api.openrouteservice.org/v2/directions/driving-car/geojson',
      {
        method: 'POST',
        headers: {
          Authorization: this.apiKey,
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          coordinates: [start, end]
        })
      }
    );

    const geo = await response.json();

    return geo.features[0].geometry.coordinates;
  }

  private async saveRoute(
    tourId: number,
    route: [number, number][]
  ) {

    const coordinates: TourCoordinate[] = route.map((p, i) => ({
      tourId,
      longitude: p[0],
      latitude: p[1],
      sequence: i
    }));

    await firstValueFrom(
      this.http.post(this.apiUrl, coordinates)
    );
  }

  private async getCoordinates(place: string): Promise<[number, number]> {

    const response = await fetch(
      `https://api.openrouteservice.org/geocode/search?api_key=${this.apiKey}&text=${encodeURIComponent(place)}`
    );

    const data = await response.json();

    return data.features[0].geometry.coordinates;
  }

  public async validatePlace(place: string): Promise<boolean> {
    try {
      await this.getCoordinates(place);
      return true;
    } catch { 
      return false;
    }
  }
}