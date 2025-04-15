import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChatService } from '../../services/chat.service';
import { Message } from '../../models/message.model';
import { UserService } from '../../services/user.service';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips'
@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatChipsModule,
  ],
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit {
  userInput = '';
  messages: Message[] = [];
  userId: string = '';
  loading = false;
  editMode = false;
  editIndex: number | null = null;

  @ViewChild('scrollContainer') private scrollContainer!: ElementRef;

  constructor(
    private chatService: ChatService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.userId = this.userService.getOrCreateUserId();
    this.messages = this.chatService.getMessages();
    this.scrollToBottom();
  }

  sendMessage(): void {
    const content = this.userInput.trim();
    if (!content) return;

    if (this.editMode && this.editIndex !== null) {
      const content = this.userInput.trim();
      if (!content) return;

      // Met à jour le message utilisateur
      this.messages[this.editIndex].content = content;
      this.messages[this.editIndex].timestamp = new Date();

      // Supprime l'ancien message bot s'il existe juste après
      const nextIndex = this.editIndex + 1;
      const hasBotAfter = this.messages[nextIndex] && this.messages[nextIndex].sender === 'bot';

      if (hasBotAfter) {
        this.messages.splice(nextIndex, 1);
      }

      this.chatService.saveMessages(this.messages);
      this.scrollToBottom();
      this.loading = true;

      // Rappel pour capturer correctement l'index dans un scope asynchrone
      const insertAt = hasBotAfter ? nextIndex : this.editIndex + 1;

      this.chatService.sendMessage(content).subscribe(response => {
        const botMessage: Message = {
          content: response,
          sender: 'bot',
          timestamp: new Date()
        };

        // 🔁 Ajoute la réponse juste après le message édité
        this.messages.splice(insertAt, 0, botMessage);

        this.chatService.saveMessages(this.messages);
        this.loading = false;
        this.scrollToBottom();
      });

      this.editMode = false;
      this.editIndex = null;
      this.userInput = '';
      return;
    }



    // Envoi normal
    const userMessage: Message = {
      content: content,
      sender: 'user',
      timestamp: new Date()
    };

    this.messages.push(userMessage);
    this.chatService.saveMessages(this.messages);
    this.scrollToBottom();

    this.loading = true;

    this.chatService.sendMessage(content).subscribe(response => {
      const botMessage: Message = {
        content: response,
        sender: 'bot',
        timestamp: new Date()
      };
      this.messages.push(botMessage);
      this.chatService.saveMessages(this.messages);
      this.loading = false;
      this.scrollToBottom();
    });

    this.userInput = '';
  }



  // resetChat(): void {
  //   this.chatService.clearMessages();
  //   this.messages = [];
  // }

  editMessage(index: number): void {
    const msg = this.messages[index];
    if (msg.sender !== 'user') return; // par sécurité

    this.editMode = true;
    this.editIndex = index;
    this.userInput = msg.content;
  }

  cancelEdit(): void {
    this.editMode = false;
    this.editIndex = null;
    this.userInput = '';
  }

  private scrollToBottom(): void {
    setTimeout(() => {
      if (this.scrollContainer) {
        this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
      }
    }, 0);
  }


}
