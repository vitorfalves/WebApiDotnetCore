import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/Evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { ptBrLocale } from 'ngx-bootstrap/locale';
import { ToastrService } from 'ngx-toastr';

defineLocale('pt-br', ptBrLocale);

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  titulo = 'Eventos';

  eventosFiltrados: Evento[];
  eventos: Evento[];
  evento: Evento;

  modoSalvar: string;

  imagemLargura = 50;
  imagemMargem = 2;
  mostrarImagem = false;
  bodyDeletarEvento: string;
  dataEvento: string;
  file: File;

  registerForm: FormGroup;

  _filtroLista: string;

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private fb: FormBuilder,
    private localeService: BsLocaleService,
    private toastr: ToastrService
    ) {
      this.localeService.use('pt-br');
     }

  get filtroLista(): string{
    return this._filtroLista;
  }
  set filtroLista(value: string){
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  editarEvento(evento: Evento, template: any){
    this.modoSalvar = "put";
    this.openModal(template);
    this.evento = Object.assign({}, evento);
    this.registerForm.patchValue(this.evento);
  }

  novoEvento(template: any){
    this.modoSalvar = "post";
    this.openModal(template);
  }

  openModal(template: any){
    this.registerForm.reset();
    template.show();
  }

  ngOnInit() {
    this.getEventos();
    this.validation();
  }

  filtrarEventos(filtrarPor: string) : Evento[]{
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      evento => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }

  alternarImagem(){
    this.mostrarImagem = !this.mostrarImagem;
  }

  validation(){
    this.registerForm = this.fb.group({
          tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]]
        , local: ['', Validators.required]
        , dataEvento: ['', Validators.required]
        , qtdPessoas: ['', [Validators.required, Validators.max(120000)]]
        , telefone: ['', Validators.required]
        , email: ['', [Validators.required, Validators.email]]
        , imagemURL: ['', Validators.required]
    });
  }

  uploadImagem(){
        const nomeArquivo = this.evento.imagemURL.split('\\', 3);
        this.evento.imagemURL = nomeArquivo[2];
        this.eventoService.postUpload(this.file, nomeArquivo[2]).subscribe(
          () => {
            this.getEventos();
          }
        );
  }

  salvarAlteracao(template: any){
    if (this.registerForm.valid){
      if (this.modoSalvar == 'post'){
        this.evento = Object.assign({}, this.registerForm.value);

        this.uploadImagem();

        this.eventoService.postEvento(this.evento).subscribe(
      (novoEvento: Evento) => {
        console.log(novoEvento);
        template.hide();
        this.getEventos();
        this.toastr.success('Evento criado com sucesso !');
      }, error => {
      this.toastr.error(`Erro ao criar evento ${error}`);
      });
      }else{
        this.evento = Object.assign({id: this.evento.id}, this.registerForm.value);

        this.uploadImagem();

        this.eventoService.putEvento(this.evento).subscribe(
      () => {
        template.hide();
        this.getEventos();
        this.toastr.success('Evento editado com sucesso !');
      }, error => {
        this.toastr.error(`Erro ao editar evento ${error}`);
      });
      }
    }
  }

  onFileChange(event){
    const reader = new FileReader();

    if(event.target.files && event.target.files.length){
      this.file = event.target.files;
      console.log(this.file);
    }
  }

  excluirEvento(evento: Evento, template: any) {
    //template.show();
    this.toastr.success('Hello world!', 'Toastr fun!');
    this.openModal(template);
    this.evento = evento;
    this.bodyDeletarEvento = `Tem certeza que deseja excluir o Evento: ${evento.tema}, Código: ${evento.id}`;
  }
  
  confirmeDelete(template: any) {
    this.eventoService.deleteEvento(this.evento.id).subscribe(
      () => {
          template.hide();
          this.getEventos();
          this.toastr.success('Deletado com sucesso !');
        }, error => {
          this.toastr.error(`Erro ao tentar deletar ${error}`);
          console.log(error);
        }
    );
  }

  getEventos(){
      this.eventoService.getEventoList().subscribe(
        (_eventos: Evento[]) => {
          this.eventos = _eventos;
          this.eventosFiltrados = this.eventos;
        },
        error => { 
          console.log(error); 
          this.toastr.error(`Erro ao tentar trazer eventos: ${error}`);
        });
  }

}
