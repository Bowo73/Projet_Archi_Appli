import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Message } from '../models/message.model';

@Injectable({ providedIn: 'root' })
export class ChatService {
  private apiUrl = 'http://localhost:5013/api/Test/generate';
  private chatKey = 'AZURE_OPENAI_API_KEY';

  constructor(private http: HttpClient) {}

  sendMessage(message: string): Observable<string> {
    const params = new HttpParams().set('prompt', message);
    return this.http.get<{ result: string }>(this.apiUrl, { params }).pipe(
      map(res => res.result)
    );
  }

  saveMessages(messages: Message[]): void {
    localStorage.setItem(this.chatKey, JSON.stringify(messages));
  }

  getMessages(): Message[] {
    const data = localStorage.getItem(this.chatKey);
    return data ? JSON.parse(data) : [];
  }

  clearMessages(): void {
    localStorage.removeItem(this.chatKey);
  }

  uploadExcel(fileData: FormData) {
    return this.http.post('http://localhost:5013/api/Import/upload', fileData);
  }
  
  downloadExcel() {
    return this.http.get('http://localhost:5013/api/Export/export', {
      responseType: 'blob'
    });
  }
  
  sendMessageWithAttachment(formData: FormData) {
    return this.http.post<{ content: string }>(
      'https://localhost:5013/api/Prompt/send',
      formData
    );
  }
}


