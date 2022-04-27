import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'readme-share-create',
  templateUrl: './readme-share-create.component.html',
  styleUrls: ['./readme-share-create.component.css']
})
export class ReadmeShareCreateComponent {
  @Output()
  onSubmit = new EventEmitter<string>();

  readmeForm: FormGroup;

  constructor(formBuilder: FormBuilder) {
    this.readmeForm = formBuilder.group({
      shareReadme: ['', [Validators.maxLength(4096)]],
    });
  }

  submit() {
    if (this.readmeForm.invalid) {
      this.readmeForm.markAllAsTouched();
      return;
    }

    this.onSubmit.emit(this.shareReadme?.value);
  }

  get shareReadme() {
    return this.readmeForm.get('shareReadme')!;
  }
}
