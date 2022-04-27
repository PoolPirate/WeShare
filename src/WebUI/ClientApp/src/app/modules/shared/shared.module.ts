import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/authservice';
import { WeShareClient } from '../../services/weshareclient';
import { NotFoundComponent } from './components/notfound/notfound.component';
import { ShareSnippetComponent } from './components/share-snippet/share-snippet.component';
import { LoadingBarRouterModule } from '@ngx-loading-bar/router';
import { MarkdownModule } from 'ngx-markdown';
import { ReadmeComponent } from './components/readme/readme.component';
import { MatIconModule } from '@angular/material/icon';
import { LikeButtonComponent } from './components/like-button/like-button.component';
import { MatButtonModule } from '@angular/material/button';
import { ForbiddenComponent } from './components/forbidden/forbidden.component';

@NgModule({
  declarations: [
    NotFoundComponent,
    ForbiddenComponent,
    ShareSnippetComponent,
    ReadmeComponent,
    LikeButtonComponent,
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([]),

    LoadingBarRouterModule,
    MarkdownModule.forRoot(),

    FormsModule,
    ReactiveFormsModule,

    MatIconModule,
    MatButtonModule,
  ],
  exports: [ 
    NotFoundComponent,
    ShareSnippetComponent,
    ReadmeComponent,
    LikeButtonComponent,

    CommonModule,

    LoadingBarRouterModule,
    MarkdownModule,

    FormsModule,
    ReactiveFormsModule,

    MatIconModule,
    MatButtonModule,
  ],
  providers: [
    AuthService,
    WeShareClient,
  ]
})
export class SharedModule { }
