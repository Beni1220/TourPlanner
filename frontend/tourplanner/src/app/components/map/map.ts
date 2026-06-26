import { AfterViewInit, Component, OnInit } from '@angular/core';
import * as L from 'leaflet';
import { TourService } from '../../services/tour.services';

@Component({
  selector: 'app-map',
  templateUrl: './map.html',
  styleUrls: ['./map.css']
})
export class MapComponent implements AfterViewInit, OnInit {
  private map!: L.Map;
  private markers: L.Marker[] = [];
  private routeLayer?: L.GeoJSON;

  constructor(private tourService: TourService) {}

  ngAfterViewInit(): void {
    delete (L.Icon.Default.prototype as any)._getIconUrl;
    L.Icon.Default.mergeOptions({
      iconRetinaUrl: 'assets/marker-icon-2x.png',
      iconUrl: 'assets/marker-icon.png',
      shadowUrl: 'assets/marker-shadow.png',
    });
    this.initMap();
  }

  ngOnInit() {
    this.tourService.tourRouteAdded.subscribe(coordinates => {
      this.drawRoute(coordinates);
    });
  }

  private initMap(): void {
    this.map = L.map('map').setView([48.2082, 16.3738], 13);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '&copy; OpenStreetMap contributors',
      crossOrigin: ''
    }).addTo(this.map);
  }

  public drawRoute(coordinates: [number, number][]): void {
    if (!coordinates || coordinates.length < 2) {
      console.error('Ungültige Route:', coordinates);
      return;
    }

    this.clearMap();

    // Koordinaten als GeoJSON aufbauen
    const geoJson = {
      type: 'Feature' as const,
      geometry: {
        type: 'LineString' as const,
        coordinates: coordinates  // [lng, lat] Format wie von ORS
      },
      properties: {}
    };

    this.routeLayer = L.geoJSON(geoJson, {
      style: { color: 'blue', weight: 5 }
    }).addTo(this.map);

    this.map.fitBounds(this.routeLayer.getBounds());

    // Start und Ziel Marker
    const start = coordinates[0];
    const end = coordinates[coordinates.length - 1];

    this.markers.push(
      L.marker([start[1], start[0]]).addTo(this.map).bindPopup('Start')
    );
    this.markers.push(
      L.marker([end[1], end[0]]).addTo(this.map).bindPopup('Ziel')
    );
  }

  public clearMap(): void {
    this.markers.forEach(m => this.map.removeLayer(m));
    this.markers = [];

    if (this.routeLayer) {
      this.map.removeLayer(this.routeLayer);
      this.routeLayer = undefined;
    }
  }
}