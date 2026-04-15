import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class OpenrouteService {

  private apiKey = 'eyJvcmciOiI1YjNjZTM1OTc4NTExMTAwMDFjZjYyNDgiLCJpZCI6IjY1ZWZkM2M2YmI5ZDQyNGI4MGI0M2M2Y2E0Zjg1M2NlIiwiaCI6Im11cm11cjY0In0=';

  async getCoordinates(place: string): Promise<[number, number]> {
    const response = await fetch(
      `https://api.openrouteservice.org/geocode/search?api_key=${this.apiKey}&text=${encodeURIComponent(place)}`
    );

    const data = await response.json();

    if (!data.features || data.features.length === 0) {
      throw new Error('Ort nicht gefunden');
    }

    return data.features[0].geometry.coordinates;
  }
}