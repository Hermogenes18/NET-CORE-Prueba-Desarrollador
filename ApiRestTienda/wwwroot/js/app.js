let productos = [];
let clientes = [];
let carrito = [];
let pedidos = [];


const formatCurrency = (amount) => {
    return new Intl.NumberFormat('es-PEN', {
        style: 'currency',
        currency: 'PEN'
    }).format(amount);
};


document.addEventListener('DOMContentLoaded', () => {
    
    cargarProductos();
    cargarClientes();
    cargarPedidos();

    
    const carritoGuardado = localStorage.getItem('carrito');
    if (carritoGuardado) {
        carrito = JSON.parse(carritoGuardado);
        actualizarCarritoUI();
    }

   
    const triggerTabList = document.querySelectorAll('#myTab button');
    triggerTabList.forEach(triggerEl => {
        triggerEl.addEventListener('click', event => {
            event.preventDefault();
            const tab = new bootstrap.Tab(triggerEl);
            tab.show();
        });
    });

    
    document.getElementById('btn-buscar').addEventListener('click', filtrarProductos);
    document.getElementById('btn-vaciar-carrito').addEventListener('click', vaciarCarrito);
    document.getElementById('btn-realizar-pedido').addEventListener('click', realizarPedido);
    document.getElementById('btn-filtrar').addEventListener('click', filtrarPedidos);
});


async function cargarProductos() {
    try {
        const response = await fetch('/api/productos');
        if (!response.ok) throw new Error('Error al cargar productos');

        productos = await response.json();
        mostrarProductos(productos);
    } catch (error) {
        console.error('Error:', error);
        alert('No se pudieron cargar los productos.');
    }
}

async function cargarClientes() {
    try {
        const response = await fetch('/api/clientes');
        if (!response.ok) throw new Error('Error al cargar clientes');

        clientes = await response.json();

        
        const clienteSelect = document.getElementById('cliente-id');
        clienteSelect.innerHTML = '<option value="">Seleccione un cliente</option>';

        
        const filtroCliente = document.getElementById('filtro-cliente');
        filtroCliente.innerHTML = '<option value="">Todos los clientes</option>';

        clientes.forEach(cliente => {
            clienteSelect.innerHTML += `<option value="${cliente.id}">${cliente.nombre}</option>`;
            filtroCliente.innerHTML += `<option value="${cliente.id}">${cliente.nombre}</option>`;
        });
    } catch (error) {
        console.error('Error:', error);
        alert('No se pudieron cargar los clientes.');
    }
}

async function cargarPedidos() {
    try {
        const response = await fetch('/api/pedidos');
        if (!response.ok) throw new Error('Error al cargar pedidos');

        pedidos = await response.json();
        mostrarPedidos(pedidos);
    } catch (error) {
        console.error('Error:', error);
        alert('No se pudieron cargar los pedidos.');
    }
}


function mostrarProductos(productosArray) {
    const listaProductos = document.getElementById('lista-productos');
    listaProductos.innerHTML = '';

    productosArray.forEach(producto => {
        const productoCard = document.createElement('div');
        productoCard.className = 'col-md-4 producto-card';
        productoCard.innerHTML = `
            <div class="card">
                <div class="card-body">
                    <h5 class="card-title">${producto.nombre}</h5>
                    <p class="card-text">Precio: ${formatCurrency(producto.precio)}</p>
                    <p class="card-text">Stock: ${producto.stock}</p>
                    <div class="d-flex justify-content-between align-items-center">
                        <div class="btn-group">
                            <button type="button" class="btn btn-outline-secondary btn-cantidad" onclick="decrementarCantidad(${producto.id})">-</button>
                            <input type="text" class="form-control mx-2" id="cantidad-${producto.id}" value="1" style="width: 60px;" readonly>
                            <button type="button" class="btn btn-outline-secondary btn-cantidad" onclick="incrementarCantidad(${producto.id}, ${producto.stock})">+</button>
                        </div>
                        <button type="button" class="btn btn-primary" onclick="agregarAlCarrito(${producto.id})">Agregar</button>
                    </div>
                </div>
            </div>
        `;
        listaProductos.appendChild(productoCard);
    });
}

function mostrarPedidos(pedidosArray) {
    const pedidosItems = document.getElementById('pedidos-items');
    pedidosItems.innerHTML = '';

    pedidosArray.forEach(pedido => {
        const cliente = clientes.find(c => c.id === pedido.clienteId) || { nombre: 'Cliente Desconocido' };
        const fecha = new Date(pedido.fecha).toLocaleDateString();

        const pedidoRow = document.createElement('tr');
        pedidoRow.className = 'pedido-row';
        pedidoRow.innerHTML = `
            <td>${pedido.id}</td>
            <td>${cliente.nombre}</td>
            <td>${fecha}</td>
            <td>${formatCurrency(pedido.total)}</td>
            <td>
                <button class="btn btn-sm btn-info" onclick="verDetallesPedido(${pedido.id})">Ver Detalles</button>
            </td>
        `;
        pedidosItems.appendChild(pedidoRow);
    });
}

function actualizarCarritoUI() {
    const carritoItems = document.getElementById('carrito-items');
    carritoItems.innerHTML = '';

    let total = 0;

    carrito.forEach((item, index) => {
        const producto = productos.find(p => p.id === item.productoId);
        if (!producto) return;

        const subtotal = producto.precio * item.cantidad;
        total += subtotal;

        const itemRow = document.createElement('tr');
        itemRow.innerHTML = `
            <td>${producto.nombre}</td>
            <td>${formatCurrency(producto.precio)}</td>
            <td>${item.cantidad}</td>
            <td>${formatCurrency(subtotal)}</td>
            <td>
                <button class="btn btn-sm btn-danger" onclick="eliminarDelCarrito(${index})">Eliminar</button>
            </td>
        `;
        carritoItems.appendChild(itemRow);
    });

    document.getElementById('carrito-total').textContent = formatCurrency(total);

    localStorage.setItem('carrito', JSON.stringify(carrito));
}

function filtrarProductos() {
    const busqueda = document.getElementById('buscar-producto').value.toLowerCase();
    const productosFiltrados = productos.filter(producto =>
        producto.nombre.toLowerCase().includes(busqueda)
    );
    mostrarProductos(productosFiltrados);
}

function filtrarPedidos() {
    const clienteId = document.getElementById('filtro-cliente').value;
    let pedidosFiltrados = pedidos;

    if (clienteId) {
        pedidosFiltrados = pedidos.filter(pedido => pedido.clienteId == clienteId);
    }

    mostrarPedidos(pedidosFiltrados);
}

function incrementarCantidad(productoId, stock) {
    const inputCantidad = document.getElementById(`cantidad-${productoId}`);
    let cantidad = parseInt(inputCantidad.value);

    if (cantidad < stock) {
        inputCantidad.value = cantidad + 1;
    }
}

function decrementarCantidad(productoId) {
    const inputCantidad = document.getElementById(`cantidad-${productoId}`);
    let cantidad = parseInt(inputCantidad.value);

    if (cantidad > 1) {
        inputCantidad.value = cantidad - 1;
    }
}

function agregarAlCarrito(productoId) {
    const producto = productos.find(p => p.id === productoId);
    if (!producto) return;

    const cantidad = parseInt(document.getElementById(`cantidad-${productoId}`).value);


    const itemExistente = carrito.findIndex(item => item.productoId === productoId);

    if (itemExistente !== -1) {
        carrito[itemExistente].cantidad += cantidad;
    } else {
        carrito.push({
            productoId,
            cantidad,
            precioUnitario: producto.precio
        });
    }

    actualizarCarritoUI();

    alert(`${cantidad} ${producto.nombre} agregado(s) al carrito`);

    document.getElementById(`cantidad-${productoId}`).value = 1;
}

function eliminarDelCarrito(index) {
    carrito.splice(index, 1);
    actualizarCarritoUI();
}

function vaciarCarrito() {
    if (confirm('¿Está seguro de vaciar el carrito?')) {
        carrito = [];
        actualizarCarritoUI();
    }
}

async function realizarPedido() {
    if (carrito.length === 0) {
        alert('El carrito está vacío');
        return;
    }

    const clienteId = document.getElementById('cliente-id').value;
    if (!clienteId) {
        alert('Debe seleccionar un cliente');
        return;
    }

    let total = 0;
    const detalles = carrito.map(item => {
        const producto = productos.find(p => p.id === item.productoId);
        const subtotal = producto.precio * item.cantidad;
        total += subtotal;

        return {
            productoId: item.productoId,
            cantidad: item.cantidad,
            precioUnitario: producto.precio,
            subtotal
        };
    });

    const pedido = {
        clienteId: parseInt(clienteId),
        fecha: new Date().toISOString(),
        total,
        detalles
    };

    try {
        const response = await fetch('/api/pedidos', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(pedido)
        });

        if (!response.ok) {
            const error = await response.json();
            throw new Error(error.message || 'Error al crear el pedido');
        }

        const pedidoId = await response.json();

        carrito = [];
        actualizarCarritoUI();

        cargarPedidos();

        alert(`Pedido #${pedidoId} creado con éxito`);

        const pedidosTab = document.getElementById('pedidos-tab');
        const tab = new bootstrap.Tab(pedidosTab);
        tab.show();

    } catch (error) {
        console.error('Error:', error);
        alert(`Error al realizar el pedido: ${error.message}`);
    }
}

async function verDetallesPedido(pedidoId) {
    try {
        const response = await fetch(`/api/pedidos/${pedidoId}`);
        if (!response.ok) throw new Error('Error al cargar detalles del pedido');

        const pedido = await response.json();

        const detallesItems = document.getElementById('detalles-pedido-items');
        detallesItems.innerHTML = '';

        pedido.detalles.forEach(detalle => {
            const producto = productos.find(p => p.id === detalle.productoId) || { nombre: 'Producto Desconocido' };

            const detalleRow = document.createElement('tr');
            detalleRow.innerHTML = `
                <td>${producto.nombre}</td>
                <td>${formatCurrency(detalle.precioUnitario)}</td>
                <td>${detalle.cantidad}</td>
                <td>${formatCurrency(detalle.subtotal)}</td>
            `;
            detallesItems.appendChild(detalleRow);
        });

        document.getElementById('detalles-pedido-total').textContent = formatCurrency(pedido.total);

        const modal = new bootstrap.Modal(document.getElementById('detallesPedidoModal'));
        modal.show();

    } catch (error) {
        console.error('Error:', error);
        alert('No se pudieron cargar los detalles del pedido.');
    }
}