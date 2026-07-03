import { Component, effect, signal } from '@angular/core';
import { TourLogsService, ITourLogs } from '../../services/tourlogs.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import {TourSelectionService} from '../../services/tour-selection-service';
import { ErrorHandlingService } from '../../services/ErrorHandlingService';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-tour-logs',
  imports: [FormsModule, CommonModule],
  templateUrl: './tour-logs.html',
  styleUrl: './tour-logs.css',
})
export class TourLogs {
  tourLogs = signal<ITourLogs[]>([]);
  error = signal<string>('');
  editingLog: ITourLogs | null = null;
  tourName = "";
  tourNames = signal<Map<number, string>>(new Map());

  newTourLog: ITourLogs = {
    tourId: 0,
    date: new Date(),
    comment: '',
    difficulty: 0,
    rating: 0,
    totalDistance: 0,
    totalTime: 0,
  };

  constructor(public tourLogsService: TourLogsService, private tourSelectionService: TourSelectionService, private errorHandlingService: ErrorHandlingService, public authService: AuthService) {
    effect(() => {
      const id = this.tourSelectionService.selectedTourId();
      this.newTourLog.tourId = id;
      this.tourName = this.tourSelectionService.selectedTourName();
      if(authService.isLoggedIn()) {
        this.loadTourLogs();
      }
    });
  }


  getTourNameByTourId(tourId: number): string {
    return this.tourNames().get(tourId) || 'Unbekannte Tour';
  }

  autoResize(event: any) {
    const el = event.target;
    el.style.height = '0px'; 
    el.style.height = el.scrollHeight + 'px';
  }

  getDifficultyLabel(difficulty: number): string {
    switch (difficulty) {
      case 0: return 'Easy';
      case 1: return 'Medium';
      case 2: return 'Hard';
      default: return 'Unknown';
    }
  }

  parseDate(value: string): Date {
    return value ? new Date(value) : new Date();
  }

  loadTourLogs(): void {
    this.tourLogsService.getToursLogs().subscribe({
      next: (logs) => {
        this.tourLogs.set(logs);
        this.loadTourNames(logs);
      },
      error: (err) => this.error.set(this.errorHandlingService.getErrorMessage(err))
    });
  }

  private loadTourNames(logs: ITourLogs[]): void {
    const uniqueIds = [...new Set(logs.map(l => l.tourId))];
    uniqueIds.forEach(id => {
      if (!this.tourNames().has(id)) {
        this.tourLogsService.getTourNameByTourId(id).subscribe({
          next: (name) => {
            const map = new Map(this.tourNames());
            map.set(id, name);
            this.tourNames.set(map);
          },
          error: (err) => this.error.set(this.errorHandlingService.getErrorMessage(err))
        });
      }
    });
  }

  addTourLog(): void {
    console.log('check 1');
    this.tourLogsService.createTourLog(this.newTourLog).subscribe({
      next: (createdLog) => { 
        console.log('check 2');
        this.tourLogs.update(logs => [...logs, createdLog]);
        console.log(this.tourSelectionService.selectedTourId());
        this.newTourLog = {
          tourId: this.tourSelectionService.selectedTourId(), // muss später auf die echte ID gesetzt werden
          date: new Date(),
          comment: '',
          difficulty: 0, 
          rating: 0,
          totalDistance: 0,
          totalTime: 0,
        };
      },
      error: (err) => this.error.set(this.errorHandlingService.getErrorMessage(err))
    });
  }

   startEdit(tourLog: ITourLogs): void {
     this.editingLog = { ...tourLog };
     this.error.set('');
   }
 
   cancelEdit(): void {
     this.editingLog = null;
   }

  saveEdit(): void {
    if (!this.editingLog || this.editingLog.id == null) {
      return;
    }

    this.tourLogsService.updateTourLog(this.editingLog).subscribe({
      next: () => {
        this.tourLogs.update(logs => logs.map(log => log.id === this.editingLog?.id ? { ...this.editingLog! } : log)); 
        this.editingLog = null;
        this.error.set('');
      },
      error: (err: any) => this.error.set(this.errorHandlingService.getErrorMessage(err))
    });

  }

  deleteTourLog(id: number): void {
    this.tourLogsService.deleteTourLog(id).subscribe({
      next: () => {
        this.tourLogs.update(logs => logs.filter(log => log.id !== id));
        if (this.editingLog?.id === id) {
          this.editingLog = null;
        }
        this.error.set('');
      },
      error: (err: any) => this.error.set(this.errorHandlingService.getErrorMessage(err))
    });
  
  }




}


