import { Component, signal } from '@angular/core';
import { TourLogsService, ITourLogs } from '../../services/tourlogs.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

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

  newTourLog: ITourLogs = {
    tourId: 1, // muss später auf die echte ID gesetzt werden
    date: new Date(),
    comment: '',
    difficulty: 0,
    rating: 0,
    totalDistance: 0,
    totalTime: 0,
  };

  constructor(private tourLogsService: TourLogsService) {
    this.loadTourLogs(); 
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
    this.tourLogsService.getTourLogs().subscribe({
      next: (logs) => this.tourLogs.set(logs),
      error: (err: any) => this.error.set('Failed to load tour logs: ' + err.message)
    });
  }

  addTourLog(): void {
    this.tourLogsService.createTourLog(this.newTourLog).subscribe({
      next: (createdLog) => { 
        this.tourLogs.update(logs => [...logs, createdLog]);
        this.newTourLog = {
          tourId: 1, // muss später auf die echte ID gesetzt werden
          date: new Date(),
          comment: '',
          difficulty: 0, 
          rating: 0,
          totalDistance: 0,
          totalTime: 0,
        };
      },
      error: (err) => this.error.set('Failed to add tour log: ' + err.message)
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
      error: (err: any) => this.error.set('Failed to update tour log: ' + err.message)
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
      error: (err: any) => this.error.set('Failed to delete tour log: ' + err.message)
    });
  
  }




}


