import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { ImageCardComponent } from './image-card.component';

/**
 * Mock image model to be used in the component
 */
const mockImageModel = jasmine.createSpyObj('image',[],{id:'test_id',url:'test_url',
  title: 'test_title', analysed: false, analysedId: 'test_analysedId'});

describe('ImageCardComponent', () => {
  let component: ImageCardComponent;
  let fixture: ComponentFixture<ImageCardComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ ImageCardComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(ImageCardComponent);
    component = fixture.componentInstance;
    component.image = mockImageModel;
    fixture.detectChanges();
  }));

  /**
   * Checks the component has been created successfully
   */
  it('should create', () => {
    expect(component).toBeTruthy();
  });

  /**
   * Checks that the image will have the correct src and will therefore display the correct image
   */
  it('should show correct image', ()=>{
    const imageUrl = fixture.debugElement.nativeElement.querySelector('ion-img').src;
    expect(imageUrl).toBe(mockImageModel.url);
  });

  /**
   * Checks that the image tittle matches the mock image model's title
   */
  it('should show correct title', ()=>{
    const cardTitle = fixture.debugElement.nativeElement.querySelector('ion-card-title').innerText.trim();
    expect(cardTitle).toBe(mockImageModel.title);
  });

  /**
   * Checks that the image's delete button calls the correct method
   */
  it('should call onDeleteImage method', ()=>{
    spyOn(component,'onDeleteImage');
    const deleteButton = fixture.debugElement.nativeElement.querySelectorAll('ion-button')[1];
    deleteButton.click();
    expect(component.onDeleteImage).toHaveBeenCalled();
  });

  /**
   * Checks that the component's delete button emits the deleteImage event
   */
  it('should emit the deleteImage event on click', ()=>{
    spyOn(component.deleteImage,'emit');
    const deleteButton = fixture.debugElement.nativeElement.querySelectorAll('ion-button')[1];
    deleteButton.click();
    expect(component.deleteImage.emit).toHaveBeenCalled();
  });

  /**
   * Checks that the deleteImage event contains the correct payload once it's emitted
   */
  it('deleteImage event should contain correct payload',  () =>{
    spyOn(component.deleteImage,'emit');
    const deleteButton = fixture.debugElement.nativeElement.querySelectorAll('ion-button')[1];
    deleteButton.click();
    expect(component.deleteImage.emit).toHaveBeenCalledWith(mockImageModel.id);
  });

  /**
   * Checks that the analyse image button press calls the correct function in the component
   */
  it('should call correct component function once pressed',  ()=> {
    spyOn(component,'analyseImage');
    const deleteButton = fixture.debugElement.nativeElement.querySelectorAll('ion-button')[0];
    deleteButton.click();
    expect(component.analyseImage).toHaveBeenCalled();

  });


  /**
   * Checks that the analyse image button press changes the analysed value of the image model
   */
  it('should change analysed value of image model',  ()=> {
    const analysed = component.image.analysed;
    component.analyseImage();
    expect(component.image.analysed).toBe(!mockImageModel.analysed);

  });
});
