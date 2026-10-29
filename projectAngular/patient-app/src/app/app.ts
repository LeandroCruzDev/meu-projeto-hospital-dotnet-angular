import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Patient, PatientService } from './services/patient-service';

@Component({
  imports: [FormsModule],
  standalone: true,
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})

export class App implements OnInit{

  private patientService = inject(PatientService);

  patients = signal<Patient[]>([]);
  newPatient: Patient = { name: '', cpf: '' };


  ngOnInit(): void {
    this.loadPatients();
  }

  loadPatients(): void{
    this.patientService.getPatients().subscribe(data => {
      this.patients.set(data);
    })
  }

  addPatient(): void {
    if(!this.newPatient.name || !this.newPatient.cpf) return;

    this.patientService.createPatient(this.newPatient).subscribe(() => {
      this.loadPatients();

      this.newPatient = { name: '', cpf: '' };
    });

  }

  deletePatiente(id?: number): void {
    if(id){
      this.patientService.deletePatient(id).subscribe(() => {
        this.loadPatients();
      })
    }
  }

  
}
