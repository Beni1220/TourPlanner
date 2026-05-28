import { AfterViewInit, Component } from '@angular/core';
import * as L from 'leaflet';
import { TourService } from '../../services/tour.services';


@Component({
  selector: 'app-map',
  templateUrl: './map.html',
  styleUrls: ['./map.css']
})
export class MapComponent implements AfterViewInit {

  private map!: L.Map;
  private markers: L.Marker[] = [];
  private routeLayer?: L.GeoJSON;


  ngAfterViewInit(): void {
    
    delete (L.Icon.Default.prototype as any)._getIconUrl;
    L.Icon.Default.mergeOptions({
      iconRetinaUrl: 'assets/marker-icon-2x.png',
      iconUrl: 'assets/marker-icon.png',
      shadowUrl: 'assets/marker-shadow.png',
    });

    this.initMap();
  }

  constructor(private tourService: TourService) {}
  
  ngOnInit() {
    this.tourService.tourRouteAdded.subscribe(coordinates => {
      this.getRoute(coordinates);
    });
  }

  private initMap(): void {
    this.map = L.map('map').setView([48.2082, 16.3738], 13); // Wien

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '&copy; OpenStreetMap contributors',
      crossOrigin: ''
    }).addTo(this.map);
  }

  public async getRoute(newRoute: any): Promise<void> {
    if (!newRoute || !newRoute[0] || !newRoute[1]) {
      console.error('Ungültige Route:', newRoute);
      return;
    }
    this.clearMap();
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

    this.routeLayer = L.geoJSON(data, {
      style: { color: 'blue', weight: 5 }
    }).addTo(this.map);

    const bounds = this.routeLayer!.getBounds();
    this.map.fitBounds(bounds);

    // route.addTo(this.map);

    this.markers.push(
      L.marker([newRoute[0][1], newRoute[0][0]]).addTo(this.map).bindPopup('Start')
    );
    this.markers.push(
      L.marker([newRoute[1][1], newRoute[1][0]]).addTo(this.map).bindPopup('Ziel')
    );
  }

  public clearMap(): void {

    // Marker löschen
    this.markers.forEach(m => this.map.removeLayer(m));
    this.markers = [];

    // Route löschen
    if (this.routeLayer) {
      this.map.removeLayer(this.routeLayer);
      this.routeLayer = undefined;
    }
  }

  public test(): void {
    const start: [number, number] = [48.2082, 16.3738]; // Wien
    const end: [number, number] = [48.3069, 14.2858]; // Linz
    this.getRoute([start, end]);
  }
  
}