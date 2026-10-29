import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";


export interface Patient {
  id?: number;
  name: string;
  cpf: string;
}

@Injectable({
  providedIn: 'root'
})

export class PatientService {
    
    private http = inject(HttpClient);

    private apiUrl = 'http://localhost:5099/api/patients';

    getPatients(): Observable<Patient[]> {
        return this.http.get<Patient[]>(this.apiUrl);
    } 

    createPatient(patient: Patient): Observable<Patient> {
        return this.http.post<Patient>(this.apiUrl, patient);
    }

    deletePatient(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }


}
