import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class UserService {
  getOrCreateUserId(): string {
    let userId = localStorage.getItem('userId');
    if (!userId) {
      userId = crypto.randomUUID();
      localStorage.setItem('userId', userId);
    }
    return userId;
  }
}
