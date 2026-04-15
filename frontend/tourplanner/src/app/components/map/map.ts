import { AfterViewInit, Component } from '@angular/core';
import * as L from 'leaflet';
import { TourService } from '../../services/tour.services';


delete (L.Icon.Default.prototype as any)._getIconUrl;

L.Icon.Default.mergeOptions({
  iconRetinaUrl: 'assets/marker-icon-2x.png',
  iconUrl: 'assets/marker-icon.png',
  shadowUrl: 'assets/marker-shadow.png',
});

@Component({
  selector: 'app-map',
  templateUrl: './map.html',
  styleUrls: ['./map.css']
})
export class MapComponent implements AfterViewInit {

  private map!: L.Map;

  ngAfterViewInit(): void {
    this.initMap();
  }
  constructor(private tourService: TourService) {}

  
  ngOnInit() {
    this.tourService.tourAdded.subscribe(coordinates => {
      this.getRoute(coordinates);
    });
  }

  private initMap(): void {
    this.map = L.map('map').setView([48.2082, 16.3738], 13); // Wien

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '&copy; OpenStreetMap contributors'
    }).addTo(this.map);
  }

  public async getRoute(newRoute: any): Promise<void> {
    
    const apiKey = 'eyJvcmciOiI1YjNjZTM1OTc4NTExMTAwMDFjZjYyNDgiLCJpZCI6IjY1ZWZkM2M2YmI5ZDQyNGI4MGI0M2M2Y2E0Zjg1M2NlIiwiaCI6Im11cm11cjY0In0=';

    const body = {
      coordinates: newRoute
    };

    const response = await fetch(
      'https://api.openrouteservice.org/v2/directions/driving-car/geojson',
      {
        method: 'POST',
        headers: {
          'Authorization': apiKey,
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(body)
      }
    );

    const data = await response.json();

    const route = L.geoJSON(data, {
      style: { color: 'blue', weight: 5 }
    });

    route.addTo(this.map);

    L.marker(newRoute[0]).addTo(this.map).bindPopup('Start');
    L.marker(newRoute[1]).addTo(this.map).bindPopup('Ziel');
  }

  public test(): void {
    const start: [number, number] = [48.2082, 16.3738]; // Wien
    const end: [number, number] = [48.3069, 14.2858]; // Linz
    this.getRoute([start, end]);
  }
  
}