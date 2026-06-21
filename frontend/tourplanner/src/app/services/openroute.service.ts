import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class OpenrouteService {

  private apiKey = 'eyJvcmciOiI1YjNjZTM1OTc4NTExMTAwMDFjZjYyNDgiLCJpZCI6IjY1ZWZkM2M2YmI5ZDQyNGI4MGI0M2M2Y2E0Zjg1M2NlIiwiaCI6Im11cm11cjY0In0=';

  //Koordianten in den backend speichern
  async getCoordinates(place: string): Promise<[number, number]> {
    const response = await fetch(
      `https://api.openrouteservice.org/geocode/search?api_key=${this.apiKey}&text=${encodeURIComponent(place)}`
    );

    if (!response.ok) {
      const errorText = await response.text();
      console.error('ORS Geocode Fehler:', errorText);
      throw new Error('Fehler bei Geocoding API');
    }

    const data = await response.json();

    if (!data || !data.features || data.features.length === 0) {
      throw new Error('Ort nicht gefunden');
    }

    const coords = data.features[0].geometry.coordinates;

    if (!coords || coords.length < 2) {
      throw new Error('Ungültige Koordinaten');
    }

    return coords;
  }
}