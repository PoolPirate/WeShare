import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'basic-share-create',
  templateUrl: './basic-share-create.component.html',
  styleUrls: ['./basic-share-create.component.css']
})
export class BasicShareCreateComponent {
  @Output()
  onSubmit = new EventEmitter<[string, string]>();

  basicForm: FormGroup;

  constructor(formBuilder: FormBuilder) {
    this.basicForm = formBuilder.group({
      shareName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(32)]],
      shareDescription: ['', [Validators.maxLength(256)]],
    });
  }

  submit() {
    const val = this.basicForm.value;

    if (val.shareName && val.shareDescription) {
      this.onSubmit.emit([val.shareName, val.shareDescription]);
    }
  }

  get shareName() {
    return this.basicForm.get('shareName')!;
  }
  get shareDescription() {
    return this.basicForm.get('shareDescription')!;
  }
}
