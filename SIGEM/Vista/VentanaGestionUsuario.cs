using SIGEM.Modelo;

namespace SIGEM.Vista;

public class VentanaGestionUsuario : Form
{
    private readonly ServicioAdministracionSistema servicio;
    private readonly Usuario usuarioActual;
    private readonly string rolSolicitado;

    private ComboBox cmbUsuarios = null!;
    private TextBox txtNombreUsuario = null!;
    private TextBox txtNombreCompleto = null!;
    private TextBox txtCorreo = null!;
    private ComboBox cmbRol = null!;
    private TextBox txtContrasena = null!;
    private TextBox txtConfirmarContrasena = null!;
    private CheckBox chkActivo = null!;
    private Button btnEditar = null!;
    private Button btnGuardar = null!;
    private Button btnEliminar = null!;

    private List<UsuarioSistemaAdministracion> usuariosRol = new();
    private bool modoEdicion;

    public VentanaGestionUsuario(
        ServicioAdministracionSistema servicio,
        Usuario usuarioActual,
        string rolSolicitado)
    {
        this.servicio = servicio;
        this.usuarioActual = usuarioActual;
        this.rolSolicitado = rolSolicitado;

        Text = $"Gestion de Usuario - {rolSolicitado}";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ClientSize = new Size(520, 580);
        BackColor = Color.FromArgb(239, 246, 255);

        ConstruirControles();
        CargarUsuarios();
        EstablecerModoEdicion(false);
    }

    private void ConstruirControles()
    {
        var pnlContenido = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            AutoScrollMinSize = new Size(480, 700),
            BackColor = Color.FromArgb(239, 246, 255)
        };
        Controls.Add(pnlContenido);

        int y = 20;

        pnlContenido.Controls.Add(CrearLabel("Seleccionar usuario", 24, y));
        y += 24;

        cmbUsuarios = new ComboBox
        {
            Location = new Point(24, y),
            Size = new Size(470, 32),
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = new Font("Segoe UI", 10F)
        };
        cmbUsuarios.SelectedIndexChanged += (_, _) => CargarUsuarioSeleccionado();
        pnlContenido.Controls.Add(cmbUsuarios);

        y += 48;
        pnlContenido.Controls.Add(CrearLabel("Nombre de usuario", 24, y));
        y += 24;
        txtNombreUsuario = CrearTextBox(24, y, 470);
        pnlContenido.Controls.Add(txtNombreUsuario);

        y += 44;
        pnlContenido.Controls.Add(CrearLabel("Nombre completo", 24, y));
        y += 24;
        txtNombreCompleto = CrearTextBox(24, y, 470);
        pnlContenido.Controls.Add(txtNombreCompleto);

        y += 44;
        pnlContenido.Controls.Add(CrearLabel("Correo electronico", 24, y));
        y += 24;
        txtCorreo = CrearTextBox(24, y, 470);
        pnlContenido.Controls.Add(txtCorreo);

        y += 44;
        pnlContenido.Controls.Add(CrearLabel("Rol", 24, y));
        y += 24;
        cmbRol = new ComboBox
        {
            Location = new Point(24, y),
            Size = new Size(470, 32),
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = new Font("Segoe UI", 10F)
        };
        cmbRol.Items.AddRange(["Administrador", "Doctor", "Enfermera", "Recepcionista"]);
        pnlContenido.Controls.Add(cmbRol);

        y += 44;
        chkActivo = new CheckBox
        {
            Text = "Usuario activo",
            Location = new Point(24, y),
            Size = new Size(200, 28),
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Checked = true
        };
        pnlContenido.Controls.Add(chkActivo);

        y += 40;
        pnlContenido.Controls.Add(CrearLabel("Nueva contrasena", 24, y));
        y += 24;
        txtContrasena = CrearTextBox(24, y, 470, password: true);
        pnlContenido.Controls.Add(txtContrasena);

        y += 44;
        pnlContenido.Controls.Add(CrearLabel("Confirmar contrasena", 24, y));
        y += 24;
        txtConfirmarContrasena = CrearTextBox(24, y, 470, password: true);
        pnlContenido.Controls.Add(txtConfirmarContrasena);

        y += 52;
        btnEditar = CrearBoton("Editar", Color.FromArgb(107, 114, 128), 24, y, 150);
        btnEditar.Click += (_, _) => EstablecerModoEdicion(true);
        pnlContenido.Controls.Add(btnEditar);

        btnGuardar = CrearBoton("Guardar", Color.FromArgb(37, 99, 235), 185, y, 150);
        btnGuardar.Click += (_, _) => GuardarCambios();
        pnlContenido.Controls.Add(btnGuardar);

        btnEliminar = CrearBoton("Eliminar", Color.FromArgb(220, 38, 38), 346, y, 148);
        btnEliminar.Click += (_, _) => EliminarUsuario();
        pnlContenido.Controls.Add(btnEliminar);
    }

    private void CargarUsuarios()
    {
        try
        {
            usuariosRol = servicio.ObtenerUsuariosPorRol(rolSolicitado);

            cmbUsuarios.DataSource = null;
            cmbUsuarios.DataSource = usuariosRol;
            cmbUsuarios.DisplayMember = nameof(UsuarioSistemaAdministracion.NombreCompleto);

            if (usuariosRol.Count > 0)
                cmbUsuarios.SelectedIndex = 0;
            else
                LimpiarCampos();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"No se pudieron cargar los usuarios.\n\nDetalle: {ex.Message}", "SIGEM",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void CargarUsuarioSeleccionado()
    {
        if (cmbUsuarios.SelectedItem is not UsuarioSistemaAdministracion usuario)
        {
            LimpiarCampos();
            return;
        }

        txtNombreUsuario.Text = usuario.NombreUsuario;
        txtNombreCompleto.Text = usuario.NombreCompleto;
        txtCorreo.Text = usuario.Correo;
        chkActivo.Checked = usuario.Activo;
        txtContrasena.Clear();
        txtConfirmarContrasena.Clear();

        string rol = NormalizarRolParaCombo(usuario.Rol);
        cmbRol.SelectedItem = cmbRol.Items.Contains(rol) ? rol : rolSolicitado;

        EstablecerModoEdicion(false);
    }

    private void EstablecerModoEdicion(bool habilitar)
    {
        modoEdicion = habilitar;

        txtNombreUsuario.ReadOnly = !habilitar;
        txtNombreCompleto.ReadOnly = !habilitar;
        txtCorreo.ReadOnly = !habilitar;
        cmbRol.Enabled = habilitar;
        chkActivo.Enabled = habilitar;
        txtContrasena.ReadOnly = !habilitar;
        txtConfirmarContrasena.ReadOnly = !habilitar;

        btnEditar.Enabled = !habilitar && cmbUsuarios.SelectedItem is not null;
        btnGuardar.Enabled = habilitar;
        btnEliminar.Enabled = cmbUsuarios.SelectedItem is not null;
    }

    private void GuardarCambios()
    {
        if (!modoEdicion)
        {
            MessageBox.Show("Presiona Editar antes de guardar cambios.", "SIGEM",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (cmbUsuarios.SelectedItem is not UsuarioSistemaAdministracion usuario)
        {
            MessageBox.Show("Selecciona un usuario.", "SIGEM",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(txtNombreUsuario.Text) ||
            string.IsNullOrWhiteSpace(txtNombreCompleto.Text))
        {
            MessageBox.Show("Completa el nombre de usuario y nombre completo.", "SIGEM",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (cmbRol.SelectedItem is null)
        {
            MessageBox.Show("Selecciona un rol.", "SIGEM",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string rolSeleccionado = cmbRol.SelectedItem.ToString() ?? rolSolicitado;

        if (usuario.IdUsuario == usuarioActual.IdUsuario &&
            string.Equals(usuario.NombreUsuario, usuarioActual.NombreUsuario, StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(rolSeleccionado, NormalizarRolParaCombo(usuario.Rol), StringComparison.OrdinalIgnoreCase))
        {
            MessageBox.Show("No puedes cambiar tu propio rol mientras tienes la sesion abierta.", "SIGEM",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!string.IsNullOrWhiteSpace(txtContrasena.Text))
        {
            if (txtContrasena.Text.Length < 6)
            {
                MessageBox.Show("La contrasena debe tener al menos 6 caracteres.", "SIGEM",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtContrasena.Text != txtConfirmarContrasena.Text)
            {
                MessageBox.Show("Las contrasenas no coinciden.", "SIGEM",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        try
        {
            bool actualizado = servicio.ActualizarUsuarioSistema(
                usuario.IdUsuario,
                txtNombreUsuario.Text,
                txtNombreCompleto.Text,
                txtCorreo.Text,
                rolSeleccionado,
                chkActivo.Checked);

            if (!actualizado)
            {
                MessageBox.Show("No se pudo actualizar el usuario.", "SIGEM",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtContrasena.Text))
            {
                bool passActualizada = servicio.CambiarContrasenaUsuarioSistema(
                    usuario.IdUsuario,
                    txtContrasena.Text);

                if (!passActualizada)
                {
                    MessageBox.Show("Se guardaron los datos, pero no se pudo actualizar la contrasena.", "SIGEM",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (usuario.IdUsuario == usuarioActual.IdUsuario &&
                string.Equals(usuario.NombreUsuario, usuarioActual.NombreUsuario, StringComparison.OrdinalIgnoreCase))
            {
                usuarioActual.NombreCompleto = txtNombreCompleto.Text.Trim();
            }

            MessageBox.Show("Usuario guardado correctamente.", "SIGEM",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            CargarUsuarios();
            EstablecerModoEdicion(false);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al guardar el usuario.\n\nDetalle: {ex.Message}", "SIGEM",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void EliminarUsuario()
    {
        if (cmbUsuarios.SelectedItem is not UsuarioSistemaAdministracion usuario)
        {
            MessageBox.Show("Selecciona un usuario.", "SIGEM",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (string.Equals(usuario.NombreUsuario, usuarioActual.NombreUsuario, StringComparison.OrdinalIgnoreCase))
        {
            MessageBox.Show("No puedes eliminar tu propia cuenta mientras tienes la sesion abierta.", "SIGEM",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show(
            $"¿Eliminar al usuario \"{usuario.NombreCompleto}\"?\n\nEsta accion no se puede deshacer.",
            "Confirmar eliminacion",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning) != DialogResult.Yes)
        {
            return;
        }

        try
        {
            bool eliminado = servicio.EliminarUsuarioSistema(usuario.IdUsuario);

            if (!eliminado)
            {
                MessageBox.Show("No se pudo eliminar el usuario.", "SIGEM",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Usuario eliminado correctamente.", "SIGEM",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            CargarUsuarios();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al eliminar el usuario.\n\nDetalle: {ex.Message}", "SIGEM",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LimpiarCampos()
    {
        txtNombreUsuario.Clear();
        txtNombreCompleto.Clear();
        txtCorreo.Clear();
        txtContrasena.Clear();
        txtConfirmarContrasena.Clear();
        chkActivo.Checked = true;
        cmbRol.SelectedItem = rolSolicitado;
        EstablecerModoEdicion(false);
    }

    private static Label CrearLabel(string texto, int x, int y)
    {
        return new Label
        {
            Text = texto,
            Location = new Point(x, y),
            AutoSize = true,
            Font = new Font("Segoe UI", 9.5F),
            ForeColor = Color.FromArgb(75, 85, 99)
        };
    }

    private static TextBox CrearTextBox(int x, int y, int ancho, bool password = false)
    {
        return new TextBox
        {
            Location = new Point(x, y),
            Size = new Size(ancho, 32),
            Font = new Font("Segoe UI", 10F),
            BorderStyle = BorderStyle.FixedSingle,
            UseSystemPasswordChar = password
        };
    }

    private static Button CrearBoton(string texto, Color color, int x, int y, int ancho)
    {
        var btn = new Button
        {
            Text = texto,
            Location = new Point(x, y),
            Size = new Size(ancho, 40),
            BackColor = color,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btn.FlatAppearance.BorderSize = 0;
        return btn;
    }

    private static string NormalizarRolParaCombo(string rol)
    {
        if (string.IsNullOrWhiteSpace(rol))
            return "Enfermera";

        string valor = rol.Trim().ToLowerInvariant();

        return valor switch
        {
            "admin" or "administrador" => "Administrador",
            "doctor" or "medico" or "médico" => "Doctor",
            "enfermera" or "enfermero" => "Enfermera",
            "recepcion" or "recepción" or "recepcionista" => "Recepcionista",
            _ => "Enfermera"
        };
    }
}
